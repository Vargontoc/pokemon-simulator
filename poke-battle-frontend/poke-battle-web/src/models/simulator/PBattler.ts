import type { Type } from "../Type"
import type { PBattlerStat, StatType } from "./PBattlerStat"

export class PBattler {
    nickname?: string
    currentHp: number = 0
    level: number = 0
    types: Type[] = []
    moves: string[] = []
    stats: PBattlerStat[] = []
    
    getStat(stat: StatType): PBattlerStat | undefined 
    {
        let result = undefined;
        this.stats.forEach((s) => {
            if(s.type !== undefined && s.type === stat) {
                result = s;
            }
        })
        return result;
    }

}