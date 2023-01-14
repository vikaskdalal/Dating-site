import {AfterViewInit, Component, ElementRef, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotificationType } from '../_common/notificationType';
import { CallNotification } from '../_models/callNotification';
import { User } from '../_models/user';
import { UserDetail } from '../_models/userDetail';
import { AccountService } from '../_services/account.service';
import { SignalRService } from '../_services/signalr.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-video-call',
  templateUrl: './video-call.component.html',
  styleUrls: ['./video-call.component.css']
})
export class VideoCallComponent implements OnInit, AfterViewInit, OnDestroy{
  friendDetails!: UserDetail;
  friendUsername!: string;
  callType!: string;
  user!: User;
  callerInfo!: CallNotification;
  localStream!: MediaStream;
  private peerConnection!: RTCPeerConnection;
  @ViewChild('localVideo') localVideo! : ElementRef;
  @ViewChild('remoteVideo') remoteVideo! : ElementRef;
  isIncomingCall!: boolean;
  isOutgoingCall!: boolean;
  callAccepted!: boolean;
  sendResponseTo!: string;
  requestVideo = false;
  isAudioMute = false;
  isVideoStopped = false;
  mediaPermissionGranted = false;
  loading =true;

  mediaConstraints = {};
  
  offerOption = {};

  constructor(
    private _route: ActivatedRoute,
    private _userService: UserService,
    private _signalrService: SignalRService,
    private _accountService: AccountService
  ) 
  { 
    this.friendUsername = this._route.snapshot.paramMap.get('username')!;
    this.isIncomingCall = this._route.snapshot.queryParamMap.has('incomingCall');
    this.isOutgoingCall = this._route.snapshot.queryParamMap.has('outgoingCall');
    this.callAccepted = this._route.snapshot.queryParamMap.has('callAccepted');
    this.sendResponseTo = this._route.snapshot.queryParamMap.get('sendResponseTo')!;
    this.requestVideo = this._route.snapshot.queryParamMap.get('requestVideo')! == 'true';
    this.callType = this.requestVideo ? NotificationType.VideoCall : NotificationType.AudioCall;

    this._accountService.currentUser$.subscribe((user) => (this.user = user!));
  }

  async ngOnInit(): Promise<void> {
    this.loadFriendsDetails();
    this.prepareMediaConstraints();
    this.isVideoStopped = !this.requestVideo;
    await this.getLocalPreview();
  }

  @HostListener('window:beforeunload')
  ngOnDestroy(): void {
      this.sendCancelCallSignal();
  }

  ngAfterViewInit(): void {
    this.sendOnLoadNotification();
    this.handleCallNotification();
  }

  loadFriendsDetails() {
    this._userService
      .getByUsername(this.friendUsername)
      .subscribe((user) => (this.friendDetails = user));
  }

  private sendOnLoadNotification() {
    this._signalrService.hubConnectionState$.subscribe(state => {
      if(!state){
        return;
      }
      if(this.isOutgoingCall)
        this.sendCallNotification();
      
      else if(this.isIncomingCall)
        this.sendAcceptedNotification();
    });
  }

  private sendAcceptedNotification(){
    if(this.callAccepted){
      this._signalrService
      .sendResponse(this.sendResponseTo, NotificationType.CallAccepted);
    }
  }

  private async getLocalPreview() {
    await navigator.mediaDevices.getUserMedia(this.mediaConstraints)
    .then(stream => {
      if(this.requestVideo)
        this.updateLocalVideo(stream);

      this.updateLocalStream(stream);
      this.mediaPermissionGranted = true;
      this.loading = false;
    })
    .catch(error => {
      this.loading = false;
      console.log(error)
      if(error.code == 0)
        this.mediaPermissionGranted = false;
    });
  }
  updateLocalStream(stream: MediaStream) {
    this.localStream = stream;
  }
  
  updateLocalVideo(stream: MediaStream): any {
    this.localVideo.nativeElement.srcObject = stream;
  }

  prepareMediaConstraints(){
    this.mediaConstraints= {
      audio : true,
      video: this.requestVideo
    };
  }

  prepareOfferOptions(){
    this.offerOption = {
      offerToReceiveAudio : true,
      offerToReceiveVideo : this.requestVideo
    };
  }

  private async SendCallOffer(){
    this.addStreamTrackToPeerConnection(this.localStream);
    this.prepareOfferOptions();
    try{
      const offer = await this.peerConnection.createOffer(this.offerOption);
      await this.peerConnection.setLocalDescription(offer);

      this._signalrService
      .sendResponse(this.callerInfo.connectionId, NotificationType.Offer, offer)
      .then(() => {
        console.log('sent offer');
        this.callAccepted = true;
      });
    }
    catch(ex){
      console.log(ex);
    }
  }

  private addStreamTrackToPeerConnection(stream: MediaStream){
    stream.getTracks().forEach(track => 
      this.peerConnection.addTrack(track, stream));
  }

  createPeerConnection() {
    this.peerConnection = new RTCPeerConnection({
      iceServers:[{
        urls: ['stun:stun1.l.google.com:19302']
      }
      ]
    })

    this.peerConnection.onicecandidate = this.handleICECandidateEvent;
    this.peerConnection.onicegatheringstatechange = this.handleIceConnectionStateChangeEvent;
    this.peerConnection.onsignalingstatechange = this.handleSignalingStateEvent;
    this.peerConnection.ontrack = this.handleTrackEvent;
  }

  private handleICECandidateEvent =  (event : RTCPeerConnectionIceEvent): void => {
    if(event.candidate){
      this._signalrService.
      sendResponse(this.callerInfo.connectionId, NotificationType.IceCandidate, event.candidate);
    }
  }

  private handleIceConnectionStateChangeEvent (event : Event): void {
   // console.log(event);

    // switch(this.peerConnection.iceConnectionState){
    //   case 'closed':
    //   case 'failed':
    //   case 'disconnected';

    // }
  }

  private handleSignalingStateEvent (event : Event): void {
    //console.log(event);
  }

  private handleTrackEvent = (event : RTCTrackEvent): void => {
    this.remoteVideo.nativeElement.srcObject = event.streams[0];
  }

  async handleIceCandidateMessage(data: any) {
    await this.peerConnection.addIceCandidate(data)
    .catch(error => console.log(error));
  }

  async handleAnswerMessage(data: any) {
    await this.peerConnection.setRemoteDescription(data);
  }

  handleCallNotification() {
    this._signalrService.callNotification$.subscribe((response) => {
      this.handleNotificationResponse(response);
    });
  }

  handleNotificationResponse(response: CallNotification) {
    this.callerInfo = response;
    
    switch(response.notificationType){
      case NotificationType.CallRejected:
        this.handleCallRejected();
        break;
      case NotificationType.CallAccepted:
        this.createPeerConnection();
        this.SendCallOffer();
        break;
      case NotificationType.Offer:
        this.handleOfferMessage(response.data);
        break;
      case NotificationType.Answer:
        this.handleAnswerMessage(response.data);
        break;
      case NotificationType.IceCandidate:
        this.handleIceCandidateMessage(response.data);
        break;
      case NotificationType.CallEnded:
        this.handkeClosePeerConnection();
        break;
    }
  }

  async handleOfferMessage(data: RTCSessionDescriptionInit) {
    if(!this.peerConnection)
      this.createPeerConnection();

    if(!this.localStream)
      await this.getLocalPreview();

    await this.peerConnection.setRemoteDescription(new RTCSessionDescription(data));
    this.addStreamTrackToPeerConnection(this.localStream);
    const answer = await this.peerConnection.createAnswer();
    await this.peerConnection.setLocalDescription(answer);

    this._signalrService.sendResponse(this.callerInfo.connectionId, NotificationType.Answer, 
      this.peerConnection.localDescription);
  }

  private handleCallRejected() {
    document.getElementById('calling_text')!.innerHTML = 'Call Rejected';
    setTimeout(() => {
      window.close();
    }, 2000);
  }

  sendCallNotification(){
    this._signalrService
      .sendCallNotification(this.friendUsername, this.callType);
  }

  cancelCall(){
    this.sendCancelCallSignal();
  }

  hangupCall(){
    this._signalrService
      .sendResponse(this.callerInfo.connectionId, NotificationType.CallEnded).then(() => {
        this.handkeClosePeerConnection();
      });
  }

  private handkeClosePeerConnection(){
    if(this.peerConnection){
      this.peerConnection.close();
    }
    window.close();
  }

  private sendCancelCallSignal(){
    this._signalrService
    .sendCallNotification(
      this.friendUsername,
      NotificationType.CallCancelled
    )
    .then(() => {
      window.close();
    });
  }

  audioToggle(){
    this.isAudioMute = !this.isAudioMute;
    const micEnabled = this.localStream.getAudioTracks()[0].enabled;
    this.localStream.getAudioTracks()[0].enabled = !micEnabled;
  }

  videoToggle(){
    this.isVideoStopped = !this.isVideoStopped;
    const videoEnabled = this.localStream.getVideoTracks()[0].enabled;
    this.localStream.getVideoTracks()[0].enabled = !videoEnabled;
  }

}
