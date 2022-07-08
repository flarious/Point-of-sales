import type { Order, OrderModel } from "./Order";

export interface Receipt {
    id: number
    code: string
    date: string
    grand_total: number
}

export interface ReceiptDetail {
    id: number
    code: string
    date: string
    total_amount: number
    total_discount_amount: number
    subtotal: number
    trade_discount: number
    grand_total: number
    orders: Order[]
}

export interface ReceiptModel {
    date: string
    total_amount: number
    total_discount_amount: number
    subtotal: number
    trade_discount: number
    grand_total: number
    orders: OrderModel[]
}