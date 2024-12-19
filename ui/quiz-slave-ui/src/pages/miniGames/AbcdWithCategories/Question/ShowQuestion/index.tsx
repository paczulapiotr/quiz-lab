type Props = {
  gameId: string;
};

const ShowQuestion = ({ gameId }: Props) => {
  return <div>{"ShowQuestion: " + gameId}</div>;
};

export default ShowQuestion;
