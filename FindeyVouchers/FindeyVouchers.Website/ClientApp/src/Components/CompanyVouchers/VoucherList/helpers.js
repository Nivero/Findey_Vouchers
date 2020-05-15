export const mapCategories = (vouchers) => {
  const categories =  vouchers.reduce((acc, curr) => {
    const { category } = curr;

    if(!acc[category.id]) {
      acc[category.id] = {
        ...category,
        vouchers: []
      }
    }

    acc[category.id].vouchers.push(curr);

    return acc;

  }, {})

  return Object.keys(categories).map((key) => categories[key]);
}
