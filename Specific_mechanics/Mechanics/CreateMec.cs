using UnityEngine;

//  dependent from IFabric, FigureInteractor, Card, IMecAction

public class CreateFigureAction : IFigureMecAction{
    //public CreateFigureAction() { 
    //    _objData = data;
    //}

    //FabricInteractor.GetFigureFabric().DestroyObject(_card.gameObject);
    public CreateFigureAction(CardDataImpl data) {
        _data = data;
    }
    public void Execute(FigureContext figureContext, ref FigureFuncDependencyContext dependencies){
        //FabricInteractor.GetFigureFabric().CreateObject(figureContext.CurrentFigure.transform.position, figureContext.CurrentFigure.Data);
        dependencies.FigureDistributor.Distribute(figureContext.CurrentFigure.gameObject.transform.position, _data);
    }

    //
    CardDataImpl _data;
}

public struct SelfDestructionAction : IFigureMecAction{
    public void Execute(FigureContext figureContext, ref FigureFuncDependencyContext dependencies){
        FabricInteractor.GetFigureFabric().DestroyObject(figureContext.CurrentFigure.gameObject);
    }
}