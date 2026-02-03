# EasyLog (CESI - Génie Logiciel)

EasyLog est une bibliothèque (DLL) destinée à centraliser la gestion des logs du projet EasySave.
Elle a été conçue pour être réutilisable et évolutive, tout en restant compatible avec les besoins des versions initiales du logiciel.

## Objectifs
- Fournir une API simple pour écrire des logs de sauvegarde en temps réel
- Garantir la compatibilité avec EasySave v1.0 (et ses évolutions)
- Permettre l’évolution du format de log selon les versions (ex: ajout de champs)

## Méthodologie Git (Workflow)
Nous utilisons le workflow suivant :

`feat/<FeatureName>` → `dev` → `main`

- **main** : branche stable (versions livrables / releases)
- **dev** : branche d’intégration (regroupe les fonctionnalités validées)
- **feat/<FeatureName>** : branche de développement d’une fonctionnalité (une feature = une branche)

### Règles de travail
1. Créer une branche à partir de `dev` :
   - `feat/<FeatureName>` (ex: `feat/json-writer`)
2. Développer et committer sur la branche `feat/...`
3. Ouvrir une **Pull Request** : `feat/...` → `dev`
4. Après validation, intégrer `dev` vers `main` via **Pull Request** (pour les versions stables / livrables)

## Règles GitHub (Branch rules)
Les règles suivantes sont appliquées (ou à appliquer) sur `main` (et idéalement sur `dev`) :

- PR obligatoire avant merge
- Minimum **1 approbation** avant merge
- Résolution des conversations obligatoire
- Force push interdit
- Suppression de branche interdite (recommandé)

## Conventions
- Conserver une compatibilité ascendante avec les versions déjà livrées
- Éviter les changements “cassants” (breaking changes) sans version majeure
- Messages de commit clairs et courts
- Commits petits et cohérents
- Toute modification significative passe par Pull Request

## Licence
Projet académique (CESI). Usage interne à l’équipe et à l’évaluation.
