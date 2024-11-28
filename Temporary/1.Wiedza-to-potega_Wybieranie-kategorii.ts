type Model = {
  rounds: {
    id: string;
    categories: {
      id: string;
      name: string;
      question: string;
      answers: {
        id: string;
        answer: string;
        isCorrect: boolean;
      }[];
    }[];
  }[];
};

const model: Model = {
  rounds: [
    {
      id: "1",
      categories: [
        {
          id: "1",
          name: "Geography",
          question: "What is the capital of Poland?",
          answers: [
            {
              id: "1",
              answer: "Warsaw",
              isCorrect: true,
            },
            {
              id: "2",
              answer: "Krakow",
              isCorrect: false,
            },
            {
              id: "3",
              answer: "Gdansk",
              isCorrect: false,
            },
            {
              id: "4",
              answer: "Wroclaw",
              isCorrect: false,
            },
          ],
        },
        {
          id: "2",
          name: "Sport",
          question: "Where is Robert Lewandowski playinh?",
          answers: [
            {
              id: "1",
              answer: "Real Madrid",
              isCorrect: false,
            },
            {
              id: "2",
              answer: "Lyon",
              isCorrect: false,
            },
            {
              id: "3",
              answer: "Barcelona",
              isCorrect: false,
            },
            {
              id: "4",
              answer: "Nice",
              isCorrect: false,
            },
          ],
        },
      ],
    },
    {
      id: "2",
      categories: [
        {
          id: "3",
          name: "History",
          question: "Who was the first president of the United States?",
          answers: [
            {
              id: "1",
              answer: "George Washington",
              isCorrect: true,
            },
            {
              id: "2",
              answer: "Thomas Jefferson",
              isCorrect: false,
            },
            {
              id: "3",
              answer: "Abraham Lincoln",
              isCorrect: false,
            },
            {
              id: "4",
              answer: "John Adams",
              isCorrect: false,
            },
          ],
        },
        {
          id: "4",
          name: "Science",
          question: "What is the chemical symbol for water?",
          answers: [
            {
              id: "1",
              answer: "H2O",
              isCorrect: true,
            },
            {
              id: "2",
              answer: "O2",
              isCorrect: false,
            },
            {
              id: "3",
              answer: "CO2",
              isCorrect: false,
            },
            {
              id: "4",
              answer: "HO",
              isCorrect: false,
            },
          ],
        },
      ],
    },
  ],
};
