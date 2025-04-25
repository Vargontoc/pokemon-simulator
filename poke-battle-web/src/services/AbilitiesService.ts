import type {  AxiosInstance } from "axios";

import type { PageResponse } from "../models/PageResponse";
import type { PageRequest } from "../models/PageRequest";
import type { AbilitiesFilter } from "../models/AbilitiesFilter";
import type { Ability } from "@/models/Ability";

export class AbilitiesService {
    private readonly http: AxiosInstance
    constructor(http: AxiosInstance) {
        this.http = http
    }


    async getAll(filter: AbilitiesFilter, request: PageRequest): Promise<PageResponse>
    {

        const res = await this.http.post<PageResponse>("/ability/query", {
            filter: filter,
            request: request
        });
        return res.data;
    }

    async get(id: number): Promise<Ability> {
        const res = await this.http.get<Ability>(`/ability/${id}`);
        
        return res.data;
    }

    async save(ability:Ability) : Promise<Ability | string[]> {

        try {
            const res = await this.http.post<Ability>("/ability", ability);
            return res.data;
        }catch(error: any) {
            if(error.response && error.response.status === 400){
                throw error.response.data.errors;
            }
            throw ["Error inesperado al guardar."];
        }
    }

    
    async update(ability:Ability) : Promise<Ability> {
        
        try {
            const res = await this.http.put<Ability>("/ability", ability);
            return res.data;
        }catch(error: any) {
            if(error.response && error.response.status === 400){
                throw error.response.data.errors;
            }
            throw ["Error inesperado al guardar."];
        }
    }
}