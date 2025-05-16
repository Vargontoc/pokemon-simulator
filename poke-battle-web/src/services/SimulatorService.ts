import { CalcExp } from "@/models/simulator/CalcExp";
import type { PBattler } from "@/models/simulator/PBattler"
import type { AxiosInstance } from "axios"

export class SimulatorService {
    private readonly http: AxiosInstance
    constructor(http: AxiosInstance) {
        this.http = http
    }

    async startBattle(): Promise<{ player: PBattler[], enemy: PBattler[]}> {
        const res =  await this.http.get(`/simulator/start`);
        return res.data;
    }

    async getCalculateExperience(code: string, level: number): Promise<number> {


        const res =  await this.http.post(`/simulator/experience/growth`, {
            code: code,
            level: level
        });
        return res.data;
    }
}