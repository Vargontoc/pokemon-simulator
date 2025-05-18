export type OperationFilter ='>' | '<' | '>=' | '<=' | '==' | '!=' | 'contains' | 'equals' | 'start-with' | 'end-with'

export function getCaptionByOperation(type: OperationFilter)
{
    switch(type) {
        case "!=":  return 'Distinto:';
        case "==": return 'Igual:';
        case ">": return 'Mayor que:';
        case "<": return 'Menor que:';
        case ">=": return 'Mayor o igual:';
        case "<=": return 'Menor o igual:';
        case "contains": return 'Contiene:';
        case "equals": return 'Igual:';
        case "start-with": return 'Empieza con:';
        case "end-with": return 'Termina con:';
        default: return '';
    }
}