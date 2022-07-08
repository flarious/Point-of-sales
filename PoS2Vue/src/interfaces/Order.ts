interface BaseOrder {
    item_code: string
    item_name: string
    item_unit: string
    item_quantity: number
    item_price: number
    item_discount_percent: number
    item_discount_amount: number
    item_total_amount: number
}

export interface Order extends BaseOrder{
    id: number
}

export interface OrderModel extends BaseOrder {
    item_id: number
    item_unit_id: number
}