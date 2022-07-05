export interface Order {
    id: number,
    item_code: string,
    item_name: string,
    item_unit: string,
    item_quantity: number,
    item_price: number,
    item_discount_percent: number,
    item_discount_amount: number,
    item_total_amount: number,
}

export interface OrderModel {
    item_id: number,
    item_code: string,
    item_name: string,
    item_unit: string,
    item_unit_id: number,
    item_quantity: number,
    item_price: number,
    item_discount_percent: number,
    item_discount_amount: number,
    item_total_amount: number,
}