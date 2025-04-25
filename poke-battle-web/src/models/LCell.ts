import type { LComboItem } from "./LComboItem"

export class LCell {
    type: LCellType = 'text'
    value: string | number | boolean |  LComboItem[] | [] | {} | undefined = undefined
}


export type LCellType = 'text'
 | 'number'
 | 'boolean' 
 | 'local-image' 
 | 'url'
 | 'combo'
 | 'icon'