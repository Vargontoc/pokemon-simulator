import type { PageRequest } from "@/models/PageRequest";
import type { PageResponse } from "@/models/PageResponse";
import type { Type } from "@/models/Type";
import type { TypesFilter } from "@/models/TypesFilter";
import type { AxiosInstance } from "axios";

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

    async save(ability:Type) : Promise<Type | string[]> {
        try {
            const res = await this.http.post<Type>("/types", ability);
            return res.data;
        }catch(error: any) {
            if(error.response && error.response.status === 400){
                throw error.response.data.errors;
            }
            throw ["Error inesperado al guardar."];
        }
    }

    
    async update(ability:Type) : Promise<Type> {
        try {
            const res = await this.http.put<Type>("/types", ability);
            return res.data;
        }catch(error: any) {
            if(error.response && error.response.status === 400){
                throw error.response.data.errors;
            }
            throw ["Error inesperado al guardar."];
        }
    }
}