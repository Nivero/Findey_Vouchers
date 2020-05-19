var stripe = Stripe("pk_test_kzGmXbmaIR7wbBaKwuzdpPrW00r5NvSJJd");
var elements = stripe.elements();

// IDEAL
// Create an instance of the idealBank Element.
var idealBank = elements.create("idealBank", {
  style: {
    base: {
      padding: "10px 12px",
      color: "#32325d",
      fontFamily:
        '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Helvetica, Arial, sans-serif',
      fontSmoothing: "antialiased",
      fontSize: "16px",
      "::placeholder": {
        color: "#aab7c4",
      },
    },
    invalid: {
      color: "#fa755a",
    },
  },
});

// Add an instance of the idealBank Element into the `ideal-bank-element` <div>.
idealBank.mount("#ideal-bank-element");

var errorMessage = document.getElementById("error-message");

// Handle form submission.
var form = document.getElementById("idl");
form.addEventListener("submit", function (event) {
  event.preventDefault();
  showLoading();

  var sourceData = {
    type: "ideal",
    amount: 1099,
    currency: "eur",
    owner: {
      name: document.querySelector('input[name="name"]').value,
    },
    // Specify the URL to which the customer should be redirected
    // after paying.
    redirect: {
      return_url: "https://shop.example.com/crtA6B28E1",
    },
  };

  // Call `stripe.createSource` with the idealBank Element and additional options.
  stripe.createSource(idealBank, sourceData).then(function (result) {
    if (result.error) {
      // Inform the customer that there was an error.
      errorMessage.textContent = result.error.message;
      errorMessage.classList.add("visible");
      stopLoading();
    } else {
      // Redirect the customer to the authorization URL.
      errorMessage.classList.remove("visible");
      stripeSourceHandler(result.source);
    }
  });
});

// // Credit card
var card = elements.create("card", {
  hidePostalCode: true,
  style: {
    base: {
      padding: 16,
      iconColor: "#666EE8",
      color: "#31325F",
      lineHeight: "40px",
      fontWeight: 300,
      fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
      fontSize: "15px",

      "::placeholder": {
        color: "#CFD7E0",
      },
    },
  },
});
card.mount("#card-element");
function setOutcome(result) {
  var successElement = document.querySelector(".success");
  var errorElement = document.querySelector(".error");
  successElement.classList.remove("visible");
  errorElement.classList.remove("visible");

  if (result.token) {
    // Use the token to create a charge or a customer
    // https://stripe.com/docs/charges
    successElement.querySelector(".token").textContent = result.token.id;
    successElement.classList.add("visible");
  } else if (result.error) {
    errorElement.textContent = result.error.message;
    errorElement.classList.add("visible");
  }
}

card.on("change", function (event) {
  setOutcome(event);
});

document.querySelector("form").addEventListener("submit", function (e) {
  e.preventDefault();
  var form = document.querySelector("form");
  var extraDetails = {
    name: form.querySelector("input[name=cardholder-name]").value,
  };
  stripe.createToken(card, extraDetails).then(setOutcome);
});
