export class LComboItem {
    key: string = ''
    value?: string
    checked?: boolean = false
    icon?: string
    tooltip?: string 
}

export type TypeOption = 'only-text' | 'only-image' | 'text-with-image'