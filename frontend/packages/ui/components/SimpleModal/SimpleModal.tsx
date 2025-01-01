import { PropsWithChildren } from "react";
import ReactModal from "react-modal";

type Props = {
  isOpen: boolean;
  onAfterOpen?: () => void;
  onAfterClose?: () => void;
} & PropsWithChildren;

const Modal = ({ children, isOpen, onAfterClose, onAfterOpen }: Props) => {
  return (
    <ReactModal
      isOpen={isOpen}
      shouldCloseOnOverlayClick={false}
      shouldFocusAfterRender
      shouldCloseOnEsc={false}
      shouldReturnFocusAfterClose
      style={{
        overlay: {
          zIndex: 1000,
          background: "transparent",
        },
        content: {
          background: "transparent",
          border: "none",
          inset: "auto",
          width: "100%",
          height: "100%",
          padding: 0,
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
        },
      }}
      onAfterOpen={onAfterOpen}
      onAfterClose={onAfterClose}
    >
      {children}
    </ReactModal>
  );
};

export default Modal;
