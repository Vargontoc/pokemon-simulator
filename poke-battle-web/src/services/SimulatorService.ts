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
}