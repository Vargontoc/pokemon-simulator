export default class BuilderBattler { 
    specie?: string;
    level?: number;
    nature?: string;
    ability?: string;
    item?: string;
    moves?: string[] = ['','','',''];
    ivs?: {item1: number, item2: number}[];
    evs?: {item1: number, item2: number}[];
}