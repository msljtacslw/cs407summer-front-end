public class TransactionPanel{
    Charactor player;
    Charactor dealer;
    public createTransaction(Charactor player, Charactor dealer){
        this.player = player;
        this.dealer = dealer;
    }

    public confirmTransaction(){
        player.addMoney();
        player.addInventory();
        dealer.addMoney();
        this.SetActive(false);
    }
}