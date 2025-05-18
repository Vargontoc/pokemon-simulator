import type { Type } from "./Type"

export class Move {
    id?: number
    internalName?: string
    name?: string
    description?: string
    power?: number
    accuracy?: number
    priority?: number
    type?: Type
    category?: string
    iconCategory?: string
}