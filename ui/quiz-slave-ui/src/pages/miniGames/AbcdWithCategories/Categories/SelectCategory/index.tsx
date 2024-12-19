type Props = {
  gameId: string;
};

const SelectCategory = ({ gameId }: Props) => {
  return <div>{"SelectCategory: " + gameId}</div>;
};

export default SelectCategory;
