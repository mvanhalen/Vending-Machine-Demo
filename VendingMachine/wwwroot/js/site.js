const connection = new signalR.HubConnectionBuilder()
    .withUrl("/vendingHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();
connection.start().catch(err => console.error(err.toString()));


var app = new Vue({
    el: '#app',
    data: {
        message: 'Insert Coins',
        machine: {products:[],coins:[],customerCoins:[]},
        totalInserted: 0

    },
    methods: {
        insertCoin: function (coin) {
            connection.invoke("ReceivedCoin", coin.cents).catch(err => console.error(err.toString()));
        },
        selectProduct: function (product) {

        },
        resetMachine: function () {

        }

    },
    computed: {
    

    },
    beforeCreate() {

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

        });
    },
    created() {
        console.log('Created');
    }
});