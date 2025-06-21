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

    async getCalculateGainedExperience(level: number, base: number, participants: number, trainer: boolean, luckyEgg: boolean) :Promise<number>
    {
        const res = await this.http.post('/simulator/experience/gained',{
            level: level,
            baseExperience: base,
            participants: participants,
            trainer: trainer,
            luckyEgg: luckyEgg
        });
        return res.data;
    }

    async getGraphGrowth(code: string) : Promise<number[]> {
        const res =  await this.http.get(`/simulator/experience/graph/${code}`);
        return res.data;
    }

    async getMultipliersOnDefense(types: string[]) : Promise<Record<number, { key: string, value: string,  icon?: string }[]>> { 
        const res = await this.http.post('/simulator/type/defender', types);
        console.log(res.data);
        return res.data;
    }

    async getMultipliersOnAttack(type: string) : Promise<Record<number, { key: string, value: string,  icon?: string }[]>> { 
        const res = await this.http.post('/simulator/type/attacker', [type]);
        console.log(res.data);
        return res.data;
    }
}