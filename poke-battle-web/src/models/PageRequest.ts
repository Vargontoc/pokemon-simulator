import type { OrderBy } from "./OrderBy"
import type { PageSize } from "./PageSize"

export class PageRequest 
{
    Page: number = 1
    PageSize: PageSize = 10
    OrderBy? : OrderBy
}