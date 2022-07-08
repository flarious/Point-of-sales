export interface ItemDetail {
    code: string
    name: string
    price: number
}

export interface ItemModel extends ItemDetail {
    unit_id: number
}

export interface Item extends ItemModel {
    id: number
    unit: string
}