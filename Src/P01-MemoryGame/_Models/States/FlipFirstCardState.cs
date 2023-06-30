namespace GameManager

{
    public class FlipFirstCardState : PlayState
    {
        public override void Update(GameManager gm)
        {
            base.Update(gm);

            var card = gm.Board.GetClickedCard();

            if (card != null) // is not
            {
                card.Flip();
                gm.FirstCard = card;
                gm.ChangeState(GameStates.FlipSecondCard);
                ScoreManager.Start();
            }
        }
    }
}
