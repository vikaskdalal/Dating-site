import {AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
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

  mediaConstraints = {};
  offerOption = {
    offerToReceiveAudio : true,
    offerToReceiveVideo : true
  };

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

  ngOnInit(): void {
    this.loadFriendsDetails();
    this.prepareMediaConstraints();
  }

  ngOnDestroy(): void {
      this.sendCancelCallSignal();
  }

  private sendAcceptedNotification(){
    if(this.callAccepted){
      this._signalrService
      .sendResponse(this.sendResponseTo, NotificationType.CallAccepted)
      .then(() => {
        //this.requestMediaDevices();
      });
    }
  }

  ngAfterViewInit(): void {
    this._signalrService.hubConnectionState$.subscribe(state => {
      if(!state){
        return;
      }
      if(this.isOutgoingCall)
        this.sendCallNotification();
      
      this.sendAcceptedNotification();
    });
    this.handleCallNotification();
    this.requestMediaDevices();
  }

  private async requestMediaDevices() {
    this.localStream = await navigator.mediaDevices.getUserMedia(this.mediaConstraints);
    this.startLocalStream();
  }

  startLocalStream(){
    this.localStream.getTracks().forEach(element => {
      element.enabled = true;
    });

    this.localVideo.nativeElement.srcObject = this.localStream;
  }

  private async makeCall(){
    this.createPeerConnection();
    if(!this.localStream)
      await this.requestMediaDevices();

    this.callAccepted = true;
    this.localStream.getTracks().forEach(track => this.peerConnection.addTrack(track, this.localStream));

    try{
      const offer = await this.peerConnection.createOffer(this.offerOption);
      this.peerConnection.setLocalDescription(offer);

      this._signalrService.sendResponse(this.callerInfo.connectionId, NotificationType.Offer, offer);
    }
    catch(ex){
      console.log(ex);
    }
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
    //console.log(event);

    if(event.candidate){
      this._signalrService.sendResponse(this.callerInfo.connectionId, NotificationType.IceCandidate, 
        event.candidate);
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

  handleIceCandidateMessage(data: any) {
    console.log('ice candidate received');
    this.peerConnection.addIceCandidate(data)
    .catch(error => console.log(error));
  }
  handleAnswerMessage(data: any) {
    console.log('answer received');
    this.peerConnection.setRemoteDescription(data);
  }

  prepareMediaConstraints(){
    this.mediaConstraints= {
      audio : true,
      video: this.requestVideo
    };
  }

  handleCallNotification() {
    this._signalrService.callNotification$.subscribe((response) => {
      this.callerInfo = response;
      // console.log('video compo response');
      // console.log(response);
      if (response.notificationType == NotificationType.CallRejected){
        this.handleCallRejected();
      } 
      else if (response.notificationType == NotificationType.CallAccepted) {
        this.makeCall();
      }
      else if(response.notificationType == NotificationType.UserNotAvailable){
        this.handleUserNotAvailable();
      }
      else if(response.notificationType == NotificationType.Offer){
        this.handleOfferMessage(response.data);
      }
      else if(response.notificationType == NotificationType.Answer){
        this.handleAnswerMessage(response.data);
      }
      else if(response.notificationType == NotificationType.IceCandidate){
        this.handleIceCandidateMessage(response.data);
      }
    });
  }

  async handleOfferMessage(data: RTCSessionDescriptionInit) {
    console.log('offer received');
    if(!this.peerConnection)
      this.createPeerConnection();

    if(!this.localStream)
      await this.requestMediaDevices();

    this.peerConnection.setRemoteDescription(new RTCSessionDescription(data))
    .then(()=>{
      this.localVideo.nativeElement.srcObject = this.localStream;

      this.localStream.getTracks().forEach(track => this.peerConnection.addTrack(track, this.localStream));
    })
    .then(()=> {
      return this.peerConnection.createAnswer();
    })
    .then(answer => {
      return this.peerConnection.setLocalDescription(answer);
    })
    .then(() => {
      console.log('answer sent');
      this._signalrService.sendResponse(this.callerInfo.connectionId, NotificationType.Answer, 
        this.peerConnection.localDescription);
    })
  }

  private handleCallRejected() {
    document.getElementById('calling_text')!.innerHTML = 'Call Rejected';
    setTimeout(() => {
      window.close();
    }, 2000);
  }

  private handleUserNotAvailable() {
    document.getElementById('calling_text')!.innerHTML = 'User is not available';
    setTimeout(() => {
      window.close();
    }, 2000);
  }

  loadFriendsDetails() {
    this._userService
      .getByUsername(this.friendUsername)
      .subscribe((user) => (this.friendDetails = user));
  }

  sendCallNotification(){
    console.log('sent notification for call');
    this._signalrService
      .sendCallNotification(this.friendUsername, this.callType)
      .then(() => {
        
      });
  }

  cancelCall(){
    this.sendCancelCallSignal();
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
