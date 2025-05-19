import { type AxiosInstance } from "axios";

export class GenAIService {
    private readonly http: AxiosInstance
    constructor(http: AxiosInstance) {
        this.http = http
    }

    async talkIA(prompt: string): Promise<any> {
        const res = await this.http.post("/talk", {
            prompt: prompt
        });
        return res.data;
    }
}