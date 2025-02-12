
type Props = {
  gameId?: string;
};

const Summary = ({gameId}: Props) => {
  return (
    <div>{gameId}</div>
  )
}

export default Summary