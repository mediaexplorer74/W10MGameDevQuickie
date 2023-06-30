namespace GameManager
{

    public class FlipSecondCardState : PlayState
    {
        public override void Update(GameManager gm)
        {
            base.Update(gm);

            var card = gm.Board.GetClickedCard();

            if (card != null && card != gm.FirstCard)
            {
                card.Flip();
                gm.SecondCard = card;
                gm.ChangeState(GameStates.ResolveTurn);
            }
        }
    }
}
