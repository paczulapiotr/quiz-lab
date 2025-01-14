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

export interface SharedMinDefMusic extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_musics';
  info: {
    description: '';
    displayName: 'MinDef.Music';
  };
  attributes: {
    rounds: Schema.Attribute.Component<'shared.min-def-music-round', true> &
      Schema.Attribute.Required;
  };
}

export interface SharedMinDefMusicRound extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_music_rounds';
  info: {
    description: '';
    displayName: 'MinDef.Music.Round';
  };
  attributes: {
    answers: Schema.Attribute.Component<
      'shared.min-def-music-round-answer',
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
  };
}

export interface SharedMinDefMusicRoundAnswer extends Struct.ComponentSchema {
  collectionName: 'components_shared_min_def_music_round_answers';
  info: {
    description: '';
    displayName: 'MinDef.Music.Round.Answer';
  };
  attributes: {
    isCorrect: Schema.Attribute.Boolean &
      Schema.Attribute.Required &
      Schema.Attribute.DefaultTo<false>;
    text: Schema.Attribute.String & Schema.Attribute.Required;
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
      'shared.min-def-music': SharedMinDefMusic;
      'shared.min-def-music-round': SharedMinDefMusicRound;
      'shared.min-def-music-round-answer': SharedMinDefMusicRoundAnswer;
      'shared.mini-game-def-abcd': SharedMiniGameDefAbcd;
      'shared.mini-game-definition': SharedMiniGameDefinition;
      'shared.rich-text': SharedRichText;
      'shared.round-definition': SharedRoundDefinition;
    }
  }
}
