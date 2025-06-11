import type BattlerMoveInfo from "./BattleMoveInfo";

export default class BattlerInfo {
    id: string = '';
    name: string = '';
    hp: number = 0;
    maxHp: number = 0;
    level: number = 1;
    hpPerentage: number = 0;
    isActive: boolean = false;
    isPlayer: boolean = true;
    wasOnBattle: boolean = false;
    ability?: string;
    item?: string;
    types:  {
        name: string,
        icon: string
    }[] = [];
    stats: {
        abbr: string,
        value: number,
        iv: number,
        ev: number,
        mod: number,
        raw: number
    }[] = []
    moves: BattlerMoveInfo[] = [];
}