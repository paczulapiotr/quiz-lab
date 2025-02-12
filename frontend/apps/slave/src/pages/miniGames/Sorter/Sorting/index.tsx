import Component from "./Component";

const Sorting = () => {
  return (
    <Component
      items={[
        { id: "1", name: "Jabłko" },
        { id: "2", name: "Ogórek" },
        { id: "3", name: "Gruszka" },
        { id: "4", name: "Marchewka" },
        { id: "5", name: "Arbuz" },
        { id: "", name: "Kalarepa" },
      ]}
      leftAnswer="Owoc"
      rightAnswer="Warzywo"
    />
  );
};

export default Sorting;
