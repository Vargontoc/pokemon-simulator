export class PBattlerStat 
{
    type?: StatType
    rawValue: number = 0
    iv: number = 0
    ev: number = 0
}

export type StatType = 'hp' | 'atk' | 'def' | 'SpAtk' | 'SpDef' | 'spd'