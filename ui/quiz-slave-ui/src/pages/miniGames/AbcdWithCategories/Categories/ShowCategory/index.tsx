type Props = {
  gameId: string;
};

const ShowCategory = ({ gameId }: Props) => {
  return <div>{"ShowCategory: " + gameId}</div>;
};

export default ShowCategory;
