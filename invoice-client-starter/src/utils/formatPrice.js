const formatPrice = (price) =>
    Number(price).toLocaleString("cs-CZ") + " Kč";

export default formatPrice;
