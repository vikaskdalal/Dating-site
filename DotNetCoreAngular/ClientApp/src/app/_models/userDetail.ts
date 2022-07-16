import { Gender } from "../_enums/gender";
import { Photo } from "./photo";

export interface UserDetail{
    name : string;
    dateOfBirth : string;
    city: string;
    gender : Gender;
    country : string;
    interests : string;
    lookingFor : string;
    introduction : string;
    email : string;
    username : string;
    photoUrl : string;
    photos : Photo[];
    created : Date;
    age : number;
}