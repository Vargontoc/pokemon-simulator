import type { LCombo } from "@/models/LCombo"
import type { AxiosInstance } from "axios"

export class ComboService {
    private readonly http: AxiosInstance
    constructor(http: AxiosInstance) {
        this.http = http
    }

    async getComboTypes() : Promise<LCombo> {
        const res = await this.http.get<LCombo>('/combos/types');
        return res.data;
    }

    async getMoveCategories() : Promise<LCombo> {
        const res = await this.http.get<LCombo>('/combos/move-categories');
        return res.data;
    }

    async getGrowth() : Promise<LCombo> {
        const res = await this.http.get<LCombo>('/combos/growths');
        return res.data;
    }

    async getSpecies() : Promise<LCombo> {
        const res = await this.http.get<LCombo>('/combos/species');
        return res.data;
    }

    async getNatures() : Promise<LCombo> {
        const res = await this.http.get<LCombo>('/combos/natures');
        return res.data;
    }

    async getAbilities() : Promise<LCombo> {
        const res = await this.http.get<LCombo>('/combos/abilities');
        return res.data;
    }

    async getMoves() : Promise<LCombo> {
        const res = await this.http.get<LCombo>('/combos/moves');
        return res.data;
    }

    async getStats() : Promise<{item1: number, item2:  string}[]> {
        const res = await this.http.get<{item1: number, item2:  string}[]>('/combos/stats');
        return res.data;
    }
}