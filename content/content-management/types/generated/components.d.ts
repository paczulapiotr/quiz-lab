import type { Schema, Struct } from '@strapi/strapi';

export interface SharedMedia extends Struct.ComponentSchema {
  collectionName: 'components_shared_media';
  info: {
    displayName: 'Media';
    icon: 'file-video';
  };
  attributes: {
    file: Schema.Attribute.Media<'images' | 'files' | 'videos'>;
  };
}

export interface SharedMinDefAbcdRoundCategory extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_abcd_round_categories';
  info: {
    description: '';
    displayName: 'MinDef.Abcd.Round.Category';
  };
  attributes: {
    name: Schema.Attribute.String & Schema.Attribute.Required;
    questions: Schema.Attribute.Component<
      'shared.min-def-abcd-round-category-question',
      true
    >;
  };
}

export interface SharedMinDefAbcdRoundCategoryQuestion
  extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_abcd_round_category_questions';
  info: {
    description: '';
    displayName: 'MinDef.Abcd.Round.Category.Question';
  };
  attributes: {
    answers: Schema.Attribute.Component<
      'shared.min-def-abcd-round-category-question-answer',
      true
    > &
      Schema.Attribute.Required &
      Schema.Attribute.SetMinMax<
        {
          max: 4;
          min: 4;
        },
        number
      >;
    audio: Schema.Attribute.Media<'files' | 'audios'>;
    text: Schema.Attribute.String &
      Schema.Attribute.Required &
      Schema.Attribute.Unique;
  };
}

export interface SharedMinDefAbcdRoundCategoryQuestionAnswer
  extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_abcd_round_category_question_answers';
  info: {
    description: '';
    displayName: 'MinDef.Abcd.Round.Category.Question.Answer';
  };
  attributes: {
    isCorrect: Schema.Attribute.Boolean &
      Schema.Attribute.Required &
      Schema.Attribute.DefaultTo<false>;
    text: Schema.Attribute.String & Schema.Attribute.Required;
  };
}

export interface SharedMinDefFamilyFeud extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_family_feuds';
  info: {
    description: '';
    displayName: 'MinDef.FamilyFeud';
  };
  attributes: {
    rounds: Schema.Attribute.Component<
      'shared.min-def-family-feud-round',
      true
    > &
      Schema.Attribute.Required &
      Schema.Attribute.SetMinMax<
        {
          min: 1;
        },
        number
      >;
  };
}

export interface SharedMinDefFamilyFeudRound extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_family_feud_rounds';
  info: {
    description: '';
    displayName: 'MinDef.FamilyFeud.Round';
  };
  attributes: {
    answers: Schema.Attribute.Component<
      'shared.min-def-family-feud-round-answer',
      true
    > &
      Schema.Attribute.Required &
      Schema.Attribute.SetMinMax<
        {
          min: 1;
        },
        number
      >;
    question: Schema.Attribute.String & Schema.Attribute.Required;
  };
}

export interface SharedMinDefFamilyFeudRoundAnswer
  extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_family_feud_round_answers';
  info: {
    description: '';
    displayName: 'MinDef.FamilyFeud.Round.Answer';
  };
  attributes: {
    answer: Schema.Attribute.String & Schema.Attribute.Required;
    points: Schema.Attribute.Integer & Schema.Attribute.Required;
    synonyms: Schema.Attribute.Component<
      'shared.min-def-family-feud-round-answer-synonym',
      true
    > &
      Schema.Attribute.SetMinMax<
        {
          min: 1;
        },
        number
      >;
  };
}

export interface SharedMinDefFamilyFeudRoundAnswerSynonym
  extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_family_feud_round_answer_synonyms';
  info: {
    description: '';
    displayName: 'MinDef.FamilyFeud.Round.Answer.Synonym';
  };
  attributes: {
    name: Schema.Attribute.String & Schema.Attribute.Required;
  };
}

export interface SharedMinDefLetters extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_letters';
  info: {
    description: '';
    displayName: 'MinDef.Letters';
  };
  attributes: {
    rounds: Schema.Attribute.Component<'shared.min-def-letters-round', true> &
      Schema.Attribute.Required &
      Schema.Attribute.SetMinMax<
        {
          min: 1;
        },
        number
      >;
  };
}

export interface SharedMinDefLettersRound extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_letters_rounds';
  info: {
    description: '';
    displayName: 'MinDef.Letters.Round';
  };
  attributes: {
    phrase: Schema.Attribute.String & Schema.Attribute.Required;
  };
}

export interface SharedMinDefMusic extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_musics';
  info: {
    description: '';
    displayName: 'MinDef.Music';
  };
  attributes: {
    rounds: Schema.Attribute.Component<'shared.min-def-music-round', true> &
      Schema.Attribute.Required &
      Schema.Attribute.SetMinMax<
        {
          min: 1;
        },
        number
      >;
  };
}

export interface SharedMinDefMusicRound extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_music_rounds';
  info: {
    description: '';
    displayName: 'MinDef.Music.Round';
  };
  attributes: {
    categories: Schema.Attribute.Component<
      'shared.min-def-music-round-category',
      true
    > &
      Schema.Attribute.Required &
      Schema.Attribute.SetMinMax<
        {
          max: 4;
          min: 2;
        },
        number
      >;
  };
}

export interface SharedMinDefMusicRoundCategory extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_music_round_categories';
  info: {
    description: '';
    displayName: 'MinDef.Music.Round.Category';
  };
  attributes: {
    name: Schema.Attribute.String & Schema.Attribute.Required;
    questions: Schema.Attribute.Component<
      'shared.min-def-music-round-category-question',
      true
    > &
      Schema.Attribute.Required &
      Schema.Attribute.SetMinMax<
        {
          min: 1;
        },
        number
      >;
  };
}

export interface SharedMinDefMusicRoundCategoryQuestion
  extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_music_round_category_questions';
  info: {
    description: '';
    displayName: 'MinDef.Music.Round.Category.Question';
  };
  attributes: {
    answers: Schema.Attribute.Component<
      'shared.min-def-music-round-category-question-answer',
      true
    > &
      Schema.Attribute.Required &
      Schema.Attribute.SetMinMax<
        {
          max: 4;
          min: 4;
        },
        number
      >;
    audio: Schema.Attribute.Media<'audios'> & Schema.Attribute.Required;
    text: Schema.Attribute.String;
  };
}

export interface SharedMinDefMusicRoundCategoryQuestionAnswer
  extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_music_round_category_question_answers';
  info: {
    description: '';
    displayName: 'MinDef.Music.Round.Category.Question.Answer';
  };
  attributes: {
    isCorrect: Schema.Attribute.Boolean &
      Schema.Attribute.Required &
      Schema.Attribute.DefaultTo<false>;
    text: Schema.Attribute.String & Schema.Attribute.Required;
  };
}

export interface SharedMinDefSorter extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_sorters';
  info: {
    displayName: 'MinDef.Sorter';
  };
  attributes: {
    rounds: Schema.Attribute.Component<'shared.min-def-sorter-round', true> &
      Schema.Attribute.Required &
      Schema.Attribute.SetMinMax<
        {
          min: 1;
        },
        number
      >;
  };
}

export interface SharedMinDefSorterRound extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_sorter_rounds';
  info: {
    description: '';
    displayName: 'MinDef.Sorter.Round';
  };
  attributes: {
    leftCategory: Schema.Attribute.Component<
      'shared.min-def-sorter-round-category',
      false
    > &
      Schema.Attribute.Required;
    rightCategory: Schema.Attribute.Component<
      'shared.min-def-sorter-round-category',
      false
    > &
      Schema.Attribute.Required;
  };
}

export interface SharedMinDefSorterRoundCategory
  extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_sorter_round_categories';
  info: {
    description: '';
    displayName: 'MinDef.Sorter.Round.Category';
  };
  attributes: {
    items: Schema.Attribute.Component<
      'shared.min-def-sorter-round-category-item',
      true
    > &
      Schema.Attribute.Required &
      Schema.Attribute.SetMinMax<
        {
          min: 1;
        },
        number
      >;
    name: Schema.Attribute.String & Schema.Attribute.Required;
  };
}

export interface SharedMinDefSorterRoundCategoryItem
  extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_sorter_round_category_items';
  info: {
    description: '';
    displayName: 'MinDef.Sorter.Round.Category.Item';
  };
  attributes: {
    name: Schema.Attribute.String & Schema.Attribute.Required;
  };
}

export interface SharedMiniGameDefAbcd extends Struct.ComponentSchema {
  collectionName: 'components_shared_mini_game_def_abcds';
  info: {
    description: '';
    displayName: 'MinDef.Abcd';
  };
  attributes: {
    rounds: Schema.Attribute.Component<'shared.round-definition', true> &
      Schema.Attribute.Required;
  };
}

export interface SharedMiniGameDefinition extends Struct.ComponentSchema {
  collectionName: 'components_shared_mini_game_definitions';
  info: {
    displayName: 'MiniGameDefinition';
  };
  attributes: {
    Description: Schema.Attribute.String &
      Schema.Attribute.Required &
      Schema.Attribute.SetMinMaxLength<{
        maxLength: 100;
      }>;
    Type: Schema.Attribute.Enumeration<
      ['AbcdWithCategories', 'Music', 'Charades']
    > &
      Schema.Attribute.Required;
  };
}

export interface SharedRichText extends Struct.ComponentSchema {
  collectionName: 'components_shared_rich_texts';
  info: {
    description: '';
    displayName: 'Rich text';
    icon: 'align-justify';
  };
  attributes: {
    body: Schema.Attribute.RichText;
  };
}

export interface SharedRoundDefinition extends Struct.ComponentSchema {
  collectionName: 'components_shared_round_definitions';
  info: {
    description: '';
    displayName: 'MinDef.Abcd.Round';
  };
  attributes: {
    categories: Schema.Attribute.Component<
      'shared.min-def-abcd-round-category',
      true
    > &
      Schema.Attribute.Required &
      Schema.Attribute.SetMinMax<
        {
          max: 4;
          min: 4;
        },
        number
      >;
  };
}

declare module '@strapi/strapi' {
  export module Public {
    export interface ComponentSchemas {
      'shared.media': SharedMedia;
      'shared.min-def-abcd-round-category': SharedMinDefAbcdRoundCategory;
      'shared.min-def-abcd-round-category-question': SharedMinDefAbcdRoundCategoryQuestion;
      'shared.min-def-abcd-round-category-question-answer': SharedMinDefAbcdRoundCategoryQuestionAnswer;
      'shared.min-def-family-feud': SharedMinDefFamilyFeud;
      'shared.min-def-family-feud-round': SharedMinDefFamilyFeudRound;
      'shared.min-def-family-feud-round-answer': SharedMinDefFamilyFeudRoundAnswer;
      'shared.min-def-family-feud-round-answer-synonym': SharedMinDefFamilyFeudRoundAnswerSynonym;
      'shared.min-def-letters': SharedMinDefLetters;
      'shared.min-def-letters-round': SharedMinDefLettersRound;
      'shared.min-def-music': SharedMinDefMusic;
      'shared.min-def-music-round': SharedMinDefMusicRound;
      'shared.min-def-music-round-category': SharedMinDefMusicRoundCategory;
      'shared.min-def-music-round-category-question': SharedMinDefMusicRoundCategoryQuestion;
      'shared.min-def-music-round-category-question-answer': SharedMinDefMusicRoundCategoryQuestionAnswer;
      'shared.min-def-sorter': SharedMinDefSorter;
      'shared.min-def-sorter-round': SharedMinDefSorterRound;
      'shared.min-def-sorter-round-category': SharedMinDefSorterRoundCategory;
      'shared.min-def-sorter-round-category-item': SharedMinDefSorterRoundCategoryItem;
      'shared.mini-game-def-abcd': SharedMiniGameDefAbcd;
      'shared.mini-game-definition': SharedMiniGameDefinition;
      'shared.rich-text': SharedRichText;
      'shared.round-definition': SharedRoundDefinition;
    }
  }
}
