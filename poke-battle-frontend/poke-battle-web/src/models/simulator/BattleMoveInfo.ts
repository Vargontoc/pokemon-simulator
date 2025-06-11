export default interface BattlerMoveInfo {
    name: string;
    displayName: string;
    pp: number;
    maxPP: number;
    power?: number;
    accuracy?: number;
    priority: number;
    effect: string;
    type: { name: string, icon: string }
    category: { name: string, icon: string }
}