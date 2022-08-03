export class UserTyping {
    public username: string;
    public isTyping: boolean;

    constructor(username: string, isTyping: boolean) {
        this.isTyping = isTyping;
        this.username = username;
    }
}