const connection = new signalR.HubConnectionBuilder()
    .withUrl("/vendingHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();
connection.start().catch(err => console.error(err.toString()));


var app = new Vue({
    el: '#app',
    data: {
        message: 'Insert coins please, no credit',
        machine: {products:[],coins:[],customerCoins:[],changeCoins:[]},
        totalInserted: 0,
        totalChange: 0

    },
    methods: {
        insertCoin: function (coin) {
            connection.invoke("ReceivedCoin", coin.cents).catch(err => console.error(err.toString()));
        },
        selectProduct: function (product) {
            connection.invoke("ReceivedSale", product).catch(err => console.error(err.toString()));
        },
        restock: function () {
            connection.invoke("Restock").catch(err => console.error(err.toString()));
        }

    },
    beforeCreate: function() {
        var vm = this;
        console.log('Before Create');      
        connection.on("ReceiveMessage", (message) => {
            console.log('Message');
            console.log(message);
            vm.machine = message;
            //update totalinserted
            vm.totalInserted = vm.machine.customerCoins.reduce(function(accumulator, coin) {
                return accumulator + coin.totalCents;
            }, 0);

            vm.totalChange = vm.machine.changeCoins.reduce(function (accumulator, coin) {
                return accumulator + coin.totalCents;
            }, 0);

           
            if (vm.totalInserted > 130) {
                this.message = "Select a product or insert more credit";
            } else if (vm.totalInserted > 0) {
                this.message = "Insert coins please, insufficent credit";
            } else
            {
                this.message = "Insert coins please, no credit";
            }

           
        });
    },
    created: function() {
        console.log('Created');
    }
});