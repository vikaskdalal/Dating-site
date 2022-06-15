import { Gender } from "../_enums/gender";

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
}