import type { PageRequest } from "@/models/PageRequest";
import type { PageResponse } from "@/models/PageResponse";
import type { Type } from "@/models/Type";
import type { TypesFilter } from "@/models/TypesFilter";
import type { AxiosInstance } from "axios";
import { useErrorModal } from "@/composables/UseErrorModal";

const { showErrorModal} = useErrorModal();
export class TypesService {
    private readonly http: AxiosInstance
    constructor(http: AxiosInstance) {
        this.http = http
    }


    async getAll(filter: TypesFilter, request: PageRequest): Promise<PageResponse>
    {

        const res = await this.http.post<PageResponse>("/types/query", {
            filter: filter,
            request: request
        });
        return res.data;
    }

    async get(id: number): Promise<Type> {
        const res = await this.http.get<Type>(`/types/${id}`);
        
        return res.data;
    }

    async save(entity:Type) : Promise<Type | string[]> {
        try {
            const res = await this.http.post<Type>("/types", entity);
            return res.data;
        }catch(error: any) {
            showErrorModal("Error", "Error inesperado al guardar el tipo.", error);
            return Promise.reject(error);
        }
    }

    
    async update(entity:Type) : Promise<Type> {
        try {
            const res = await this.http.put<Type>("/types", entity);
            return res.data;
        }catch(error: any) {
            showErrorModal("Error", "Error inesperado al actualizar el tipo.", error);
            return Promise.reject(error);
        }
    }

        async delete(id: number): Promise<Boolean> {
        try {
            const res = await this.http.delete<boolean>(`/types/${id}`);
            return res.data;
        } catch (error: any) {
            showErrorModal("Error al eliminar la habilidad", "Error inesperado al eliminar el tipo.", error);
            return Promise.reject(error);
        }
    }
}