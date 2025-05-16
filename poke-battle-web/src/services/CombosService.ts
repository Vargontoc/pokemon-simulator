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
}