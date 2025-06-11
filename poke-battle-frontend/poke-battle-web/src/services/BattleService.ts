import type BattlerInfoResponse from "@/models/simulator/BattlerInfoResponse";
import BattleTurn from "@/models/simulator/BattleTurn";
import type BuildBattle from "@/models/simulator/BuildBattle"
import type { AxiosInstance } from "axios"

export default class BattleService {


    private readonly http: AxiosInstance
    constructor(http: AxiosInstance) {
        this.http = http
    }

    async startBattle(build: BuildBattle) : Promise<string> {
        try {
            const res = await this.http.post<string>("/battle/new", build);
            return res.data;
        } catch (error: any) {
            if (error.response && error.response.status === 400) {
                throw error.response.data.errors;
            }
            throw ["Error inesperado al iniciar la batalla."];
        }
    }

    async getBattleInfo(id: string) : Promise<BattlerInfoResponse> {
        try {
            const res = await this.http.get<BattlerInfoResponse>(`/battle/${id}`);
            return res.data as BattlerInfoResponse;
        } catch (error: any) {
            if (error.response && error.response.status === 404) {
                throw ["Batalla no encontrada."];
            }
            throw ["Error inesperado al obtener la información de la batalla."];
        }
    }

    async attack(id: string, move: string) : Promise<BattleTurn> {
                try {
            const res = await this.http.post<{}>(`/battle/${id}/turn`, {
                Action: 'Attack',
                Value: move
            });
            return res.data as BattleTurn;
        } catch (error: any) {
            if (error.response && error.response.status === 404) {
                throw ["Batalla no encontrada."];
            }
            throw ["Error inesperado al obtener la información de la batalla."];
        }
    }


    async switch(id: string, idBattler: string) {
        try {
            const res = await this.http.post<{}>(`/battle/${id}/turn`, {
                Action: 'Switch',
                Value: idBattler
            });
            return res.data as BattleTurn;
        } catch (error: any) {
            if (error.response && error.response.status === 404) {
                throw ["Batalla no encontrada."];
            }
            throw ["Error inesperado al obtener la información de la batalla."];
        }
    }
}