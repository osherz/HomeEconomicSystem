﻿namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    public enum States
    {
        MainState,
        Draft,
        GraphCreatingForDraft,
        GraphCreatingForFavorite,
        SavingNewGraphDraft,
        SavingNewGraphFavorite,
        NewGraphDraftSaved,
        SavingChanges,
        AddingToFavorites,
        AddedToFavorites,
        ClearingAll,
        Cleared,
        FavoritesEditing,
        AssociationRules,
        Favorites,
        DeleteFavorite,
        ChangesSaved,
        FavoriteCreatingCanceled,
        DraftCreatingCanceled,
        NewGraphFavoriteSaved,
        FavoriteDeleted
    }
}
