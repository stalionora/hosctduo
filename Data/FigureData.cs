public class FigureData {
    public MovementWay Way => _way;
    
    public void SetWay(MovementWay way) {
        _way = way;
    }

    //
    private MovementWay _way;
}