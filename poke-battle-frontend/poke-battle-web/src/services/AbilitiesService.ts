import type {  AxiosInstance } from "axios";

import type { PageResponse } from "../models/PageResponse";
import type { PageRequest } from "../models/PageRequest";
import type { AbilitiesFilter } from "../models/AbilitiesFilter";
import type { Ability } from "@/models/Ability";
import { useErrorModal } from "@/composables/UseErrorModal";

const { showErrorModal} = useErrorModal();
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
            showErrorModal("Error", "Error inesperado al guardar la habilidad.", error);
            return Promise.reject(error);
        }
    }

    
    async update(ability:Ability) : Promise<Ability> {
        
        try {
            const res = await this.http.put<Ability>("/ability", ability);
            return res.data;
        }catch(error: any) {
            showErrorModal("Error", "Error inesperado al actualizar la habilidad.", error);
            return Promise.reject(error);
        }
    }

    async delete(id: number): Promise<Boolean> {
        try {
            const res = await this.http.delete<boolean>(`/ability/${id}`);
            return res.data;
        } catch (error: any) {
            showErrorModal("Error", "Error inesperado al eliminar la habilidad.", error);
            return Promise.reject(error);
        }
    }

    async generateAbilities(): Promise<Ability> { 
        try {
            const res = await this.http.get<Ability>("/ability/generate");
            return res.data;
        } catch (error: any) {
            showErrorModal("Error", "Error inesperado al generar una habilidad.", error);
            return Promise.reject(error);
        }

    }

}