import streamlit as st
from services.blob_service import upload_blob
from services.credit_card_service import analize_credit_card_info

def configure_interface():
    st.title("Upload de arquivos DIO - Desafio 1 - Azure Fake Docs")
    uploaded_file = st.file_uploader("escolha um arquivo", type=["png","jpg","jpeg"])

if uploaded_file is not None:
    fileName = uploaded_file.name

    blob_url = upload_blob(upload_file,fileName)
    if blob_url:
        st.write(f"arquivo{fileName} enviado com sucesso para ")
        credit_card_info = analize_credit_card_info(blob_url)
        show_image_and_validation(blob_url, credit_card_info)
    else:
        st.write(f"erro ao enviar")

def show_image_and_validation():
    st.image(blob_url, caption = "imagem enviada", use_column_width=True)
    st.write("informaç~eos de cartão de credito encontradas")
    st.write(credit_card_info)
    if (credit_card_info and credit_card_info["card_name"]):
        st.markdown(f"<h1 style='color: green;'> Cartão valido</h1>", unsafe_allow_html=True)
        st.write(f"Nome do titular:{credit_card_info['card_name']}")
        st.write(f"Banco Emissor:{credit_card_info['bank_name']}")
        st.write(f"Data de Validade:{credit_card_info['expiry_date']}")
    else:
        st.write(f"Cartão invalido")

if __name__ == "__main__":
    configure_interface()




