import type { Move } from "@/models/Move";
import type { MovesFilter } from "@/models/MovesFilter";
import type { PageRequest } from "@/models/PageRequest";
import type { PageResponse } from "@/models/PageResponse";
import type { AxiosInstance } from "axios";

export class MovesService {
    private readonly http: AxiosInstance
    constructor(http: AxiosInstance) {
        this.http = http
    }


    async getAll(filter: MovesFilter, request: PageRequest): Promise<PageResponse>
    {

        const res = await this.http.post<PageResponse>("/move/query", {
            filter: filter,
            request: request
        });
        return res.data;
    }

    async get(id: number): Promise<Move> {
        const res = await this.http.get<Move>(`/move/${id}`);
        
        return res.data;
    }

    async save(move:Move) : Promise<Move | string[]> {
        try {
            const res = await this.http.post<Move>("/move", move);
            return res.data;
        }catch(error: any) {
            if(error.response && error.response.status === 400){
                throw error.response.data.errors;
            }
            throw ["Error inesperado al guardar."];
        }
    }

    
    async update(move:Move) : Promise<Move> {
        try {
            const res = await this.http.put<Move>("/move", move);
            return res.data;
        }catch(error: any) {
            if(error.response && error.response.status === 400){
                throw error.response.data.errors;
            }
            throw ["Error inesperado al guardar."];
        }
    }
}