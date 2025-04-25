import type { Ability } from "./Ability"
import type { Type } from "./Type"

export class PageResponse {
    page: number = 1
    pageSize: number = 10
    total: number = 1
    results: Ability[] | Type[] = []
}