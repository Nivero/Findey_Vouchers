import { CART_ADD, CART_REMOVE, CART_RESET } from './actions'

function reducer(state, action){
    let priceReduced;
    switch (action.type) {
        case CART_ADD:
            if(action.payload.voucher.discount > 0){
                priceReduced = (action.payload.voucher.price * (1-(action.payload.voucher.discount/100))).toFixed(2);
            }
            let tmpCartItems = state.cartItems.map((cartItem) => {
                if(cartItem.id === action.payload.voucher.id){
                    if(action.payload.voucher.voucherType == 1){
                        cartItem = { ...cartItem, amount: cartItem.amount ? cartItem.amount + 1 : 1, prepaidAmount : +cartItem.prepaidAmount - +cartItem.price}
                    } else {
                        cartItem = { ...cartItem, amount: cartItem.amount ? cartItem.amount + 1 : 1}
                    }
                }
                return cartItem
            });
            return {
                ...state,
                cartItems: tmpCartItems,
                cartTotal: (Math.round(((priceReduced > 0) ? +state.cartTotal + +priceReduced : +state.cartTotal + action.payload.voucher.price )* 100) / 100).toFixed(2),
                cartAmount: state.cartAmount + 1
            }
        case CART_REMOVE:
            if(action.payload.voucher.discount > 0){
                priceReduced = (action.payload.voucher.price * (1-(action.payload.voucher.discount/100))).toFixed(2);
            }
            return {
                ...state,
                cartItems: state.cartItems.filter((cartItem) => { return cartItem.id !== action.payload.voucher.id }),
                cartTotal: (Math.round(((priceReduced > 0) ? +state.cartTotal - +priceReduced : +state.cartTotal - action.payload.voucher.price )* 100) / 100).toFixed(2),
                cartAmount: state.cartAmount -1
            }
        case CART_RESET:
            return {
                ...state,
                cartItems: state.cartItems.map((cartItem) => { cartItem.amount = 0; return cartItem}),
                cartTotal: 0, cartAmount: 0}
        default:
            break;
    }
    return state;
}

export default reducer;