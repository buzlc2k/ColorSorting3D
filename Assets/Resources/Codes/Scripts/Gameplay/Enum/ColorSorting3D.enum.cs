namespace ColorSorting3D.Gameplay
{
    public enum DataModelID
    {
        None = 0,
        Progress,
        Recources,
    }

    public enum ModalContentID
    {
        None = 0,
        Setting,
        RemoveAds,
        AddCoins,
        ConfirmPurchaseUndo,
        ConfirmPurchaseContainer,
    }

    public enum SelectionResult
    {
        None = 0,
        Selected,
        SelectedReadOnlyContainer,
        Transferred,
        TransferredFailed
    }

    public enum DispatcherState
    {
        None,
        Normal,
        Selecting,
    }
}